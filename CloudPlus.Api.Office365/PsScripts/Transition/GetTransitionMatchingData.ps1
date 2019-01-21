<#
.SYNOPSIS

This script is used to perform a check on an O365 tenant to determine what their recommended product is and whether it already exists
as a user in the Control Panel database. It is intended for the development team to take this information as the basis for an end-user
page which would allow a user who is importing an O365 tenant to see what work needs to be done and what options they want to set for
the users that need to be created.

.DESCRIPTION


.PARAMETER Domain

Any domain belong to the O365 tenant and CP customer.
#>

param($AdminUsername,$AdminPassword,$SQLServerIP,$SQLServerUsername,$SQLServerPassword,$AuthDBName,$CPDBName,$Office365CustomerId,$Domain)

##############################################
# Configurable values
##################################################
#TODO
#$ScriptPath = Split-Path $MyInvocation.MyCommand.Path -Parent

$AuthDBConnectionString = "server=VMROOT002STG4.cloudplusservices.stg;initial catalog=CloudPlusAuth;database=CloudPlusAuth;User Id=cloudplusv2; password=DhK6t7D49mVXjM3r;trusted_connection=yes;MultipleActiveResultSets=true;App=EntityFramework"
$CPDBConnectionString = "data source=VMROOT002STG4.cloudplusservices.stg;initial catalog=CloudPlusDb;integrated security=True;database=CloudPlusAuth;User Id=cloudplusv2; password=DhK6t7D49mVXjM3r;MultipleActiveResultSets=True;App=EntityFramework"


#$AuthDBConnectionString = "Server=$SQLServerIP;Database=$AuthDBName;User Id=$SQLServerUsername;Password=$SQLServerPassword"
#$CPDBConnectionString = "Server=$SQLServerIP;Database=$CPDBName;User Id=$SQLServerUsername;Password=$SQLServerPassword"

# Here are the SKUs which are currently supported by CloudPlus.
# The commands that generated these:
# $TenantID = (Get-MsolPartnerContract -DomainName cldp365.tech).TenantId
# foreach ($sku in (Get-MsolAccountSku -TenantId $TenantID)) { Write-Host $sku.AccountSkuId; foreach ($ServiceStatus in $sku.ServiceStatus) { Write-Host $ServiceStatus.ServicePlan.ServiceName } }
# These are sorted in order of cost per-user.
$ValidO365SKUFeatures = [ordered]@{
    # $1.60/user list price
    "EXCHANGEDESKLESS" = @("INTUNE_O365", "EXCHANGE_S_DESKLESS")
    # $3.20/user list price
    "EXCHANGESTANDARD" = @("INTUNE_O365","EXCHANGE_S_STANDARD")
    # $4.00/user list price
    "O365_BUSINESS_ESSENTIALS" = @("FLOW_O365_P1", "POWERAPPS_O365_P1", "TEAMS1", "PROJECTWORKMANAGEMENT", "SWAY", "INTUNE_O365", "SHAREPOINTWAC", "YAMMER_ENTERPRISE", "EXCHANGE_S_STANDARD", "MCOSTANDARD", "SHAREPOINTSTANDARD")
    # $6.40/user list price
    "STANDARDPACK" = @("Deskless", "FLOW_O365_P1", "POWERAPPS_O365_P1", "TEAMS1", "SHAREPOINTWAC", "PROJECTWORKMANAGEMENT", "SWAY", "INTUNE_O365", "YAMMER_ENTERPRISE", "MCOSTANDARD", "SHAREPOINTSTANDARD", "EXCHANGE_S_STANDARD")
    # $6.40/user list price
    "EXCHANGEENTERPRISE" = @("INTUNE_O365","EXCHANGE_S_ENTERPRISE")
    # $6.60/user list price
    "O365_BUSINESS" = @("EXCHANGE_S_FOUNDATION", "SWAY", "INTUNE_O365", "SHAREPOINTWAC", "ONEDRIVESTANDARD", "OFFICE_BUSINESS")
    # $9.60/user list price
    "OFFICESUBSCRIPTION" = @("EXCHANGE_S_FOUNDATION", "SHAREPOINTWAC", "SWAY", "INTUNE_O365", "ONEDRIVESTANDARD", "OFFICESUBSCRIPTION")
    # $10/user list price
    "O365_BUSINESS_PREMIUM" = @("FLOW_O365_P1", "POWERAPPS_O365_P1", "O365_SB_Relationship_Management", "TEAMS1", "PROJECTWORKMANAGEMENT", "SWAY", "INTUNE_O365", "SHAREPOINTWAC", "OFFICE_BUSINESS", "YAMMER_ENTERPRISE", "EXCHANGE_S_STANDARD", "MCOSTANDARD", "SHAREPOINTSTANDARD")
    # $16/user list price
    "ENTERPRISEPACK" = @("Deskless", "FLOW_O365_P2", "POWERAPPS_O365_P2", "TEAMS1", "PROJECTWORKMANAGEMENT", "SWAY", "INTUNE_O365", "YAMMER_ENTERPRISE", "RMS_S_ENTERPRISE", "OFFICESUBSCRIPTION", "MCOSTANDARD", "SHAREPOINTWAC", "SHAREPOINTENTERPRISE", "EXCHANGE_S_ENTERPRISE")
    # $26.40/user list price
    "ENTERPRISEPREMIUM_NOPSTNCONF" = @("THREAT_INTELLIGENCE", "Deskless", "FLOW_O365_P3", "POWERAPPS_O365_P3", "TEAMS1", "ADALLOM_S_O365", "EQUIVIO_ANALYTICS", "LOCKBOX_ENTERPRISE", "EXCHANGE_ANALYTICS", "SWAY", "ATP_ENTERPRISE", "MCOEV", "BI_AZURE_P2", "INTUNE_O365", "PROJECTWORKMANAGEMENT", "RMS_S_ENTERPRISE", "YAMMER_ENTERPRISE", "OFFICESUBSCRIPTION", "MCOSTANDARD", "EXCHANGE_S_ENTERPRISE", "SHAREPOINTENTERPRISE", "SHAREPOINTWAC")
    #"EOP_ENTERPRISE" = @("EOP_ENTERPRISE")
}
$ValidO365SKUNames = [ordered]@{
    "EXCHANGEDESKLESS" = "Office 365 Kiosk"
    "EXCHANGESTANDARD" = "Office 365 Exchange Online (Plan 1)"
    "O365_BUSINESS_ESSENTIALS" = "Office 365 Business Essentials"
    "STANDARDPACK" = "Office 365 Enterprise E1"
    "EXCHANGEENTERPRISE" = "Office 365 Exchange Online (Plan 2)"
    "O365_BUSINESS" = "Office 365 Business"
    "OFFICESUBSCRIPTION" = "Office 365 ProPlus"
    "O365_BUSINESS_PREMIUM" = "Office 365 Business Premium"
    "ENTERPRISEPACK" = "Office 365 Enterprise E3"
    "ENTERPRISEPREMIUM_NOPSTNCONF" = "Office 365 Enterprise E5 (without PSTN Conferencing)"
    #"EOP_ENTERPRISE" = "Office 365 Exchange Online Protection"
}
$ValidO365SKUOfferIDs = [ordered]@{
    "EXCHANGEDESKLESS" = "35A36B80-270A-44BF-9290-00545D350866"
    "EXCHANGESTANDARD" = "195416C1-3447-423A-B37B-EE59A99A19C4"
    "O365_BUSINESS_ESSENTIALS" = "BD938F12-058F-4927-BBA3-AE36B1D2501C"
    "STANDARDPACK" = "91FD106F-4B2C-4938-95AC-F54F74E9A239"
    "EXCHANGEENTERPRISE" = "2F707C7C-2433-49A5-A437-9CA7CF40D3EB"
    "O365_BUSINESS" = "5C9FD4CC-EDCE-44A8-8E91-07DF09744609"
    "OFFICESUBSCRIPTION" = "BE57FF4C-100C-4F1F-B82D-F1C5AB63A665"
    "O365_BUSINESS_PREMIUM" = "031C9E47-4802-4248-838E-778FB1D2CC05"
    "ENTERPRISEPACK" = "796B6B5F-613C-4E24-A17C-EBA730D49C02"
    "ENTERPRISEPREMIUM_NOPSTNCONF" = "4F7ECAF1-E9D6-4CAC-9687-E22EB3DFDD70"
    #"EOP_ENTERPRISE" = "D903A2DB-BF6F-4434-83F1-21BA44017813"
}

##################################################
# Simple output functions
##################################################
function ShowErrorAndStop ($errorString) {
    Write-Host -ForegroundColor Red "$errorString `r"
    Exit
}
function ShowError ($errorString) {
    Write-Host -ForegroundColor Red "$errorString `r"
}
function ShowMessage ($message, $includeTimestamp) {
    $currentTime = (Get-Date -Format G).ToString()
    if ($includeTimestamp -eq $true) {
        Write-Host -ForegroundColor Green "($currentTime) $message `r"
    } else {
        Write-Host -ForegroundColor Green "$message `r"
    }
}
function ShowWarning ($message) {
    Write-Host -ForegroundColor Yellow "$message `r"
}
function Trim-Spaces ($StringToTrim) {
    if ($StringToTrim) {
        return $StringToTrim.Trim()
    } else {
        return ""
    }
}    

##################################################
# This function takes the customer domain that was passed in
# to return the customer ID from the CP database.
##################################################
function Get-CustomerIDByDomain {
    Param(
        [string]$Domain
        )

    # Get the data from the CP database
    $sqlQuery = "SELECT * FROM Domains WHERE [Name] = '$Domain'"
    $SQLCommand_DomainLookup = New-Object System.Data.SqlClient.SqlCommand
    $SQLCommand_DomainLookup.Connection = $CPDatabaseConnection
    $SQLCommand_DomainLookup.CommandText = $sqlQuery
    $SQLReader_DomainLookup = $SQLCommand_DomainLookup.ExecuteReader()

    $DomainLookupData = New-Object System.Data.DataTable
    $DomainLookupData.Load($SQLReader_DomainLookup)

    if ($DomainLookupData.Rows.Count -gt 0) {
        $CID = $DomainLookupData.Rows[0].Company_Id
    } else {
        throw "Unable to find domain $Domain in Control Panel."
    }

    $SQLCommand_DomainLookup.Dispose()

    return $CID
}    

##################################################
# This function takes the customer ID that was passed in
# to return the customer Users table from the CP database.
##################################################
function Get-CPUsersByCustomerID {
    Param(
        [int]$CustomerID
        )

    # Get the data from the CP database
    $sqlQuery = "SELECT * FROM Users WHERE CompanyId = $CustomerID"
    $SQLCommand_UsersLookup = New-Object System.Data.SqlClient.SqlCommand
    $SQLCommand_UsersLookup.Connection = $AuthDatabaseConnection
    $SQLCommand_UsersLookup.CommandText = $sqlQuery
    $SQLReader_UsersLookup = $SQLCommand_UsersLookup.ExecuteReader()

    $UsersLookupData = New-Object System.Data.DataTable
    $UsersLookupData.Load($SQLReader_UsersLookup)

    $SQLCommand_UsersLookup.Dispose()

    return $UsersLookupData
}    

##################################################
# MAIN SCRIPT LOGIC
##################################################
# Open a connection to the CP and Auth databases
$AuthDatabaseConnection = New-Object System.Data.SqlClient.SqlConnection
$AuthDatabaseConnection.ConnectionString = $AuthDBConnectionString
$AuthDatabaseConnection.Open()
$CPDatabaseConnection = New-Object System.Data.SqlClient.SqlConnection
$CPDatabaseConnection.ConnectionString = $CPDBConnectionString
$CPDatabaseConnection.Open()

# Get the customer ID from Mongo based on the domain
$CustomerID = Get-CustomerIDByDomain -Domain $Domain

# Get the Tenant ID for this customer. Stop if not found.
$TenantID = $Office365CustomerId
if ($TenantID -eq 'False') {
    ShowErrorAndStop "Unable to find tenant with domain $Domain in Office 365. Stopping."
}

# Make sure the domain is verified. Stop if it isn't.
#$DomainVerificationStatus = [System.Convert]::ToBoolean((& $ScriptPath\Check-O365DomainVerificationStatus.ps1 $Domain))
#if ($DomainVerificationStatus -eq $false) {
#    ShowErrorAndStop "All customer domains have not been verified in Office 365. Domains must be validated before proceeding."
#}

# Connect to the Office 365 service.
$O365Cred = New-Object -TypeName System.Management.Automation.PSCredential -ArgumentList $AdminUsername,(ConvertTo-SecureString -AsPlainText -Force -String $AdminPassword)
try {
    Import-Module MSOnline

    Connect-MsolService -Credential $O365Cred -ErrorAction Stop
} catch {
    throw "Unable to authenticate to Office 365 Service with error `"$($_.Exception.Message)`"."
}

# Import the CSV so we know what subscriptions we need to add to the customer.
$O365Users = @(Get-MsolUser -TenantId $TenantID | ? {$_.UserPrincipalName -notmatch '.onmicrosoft.com'})

# Get the list of users already in this customer.
$UserRecords = Get-CPUsersByCustomerID -CustomerID $CustomerID

# Get all of the SKUs belonging to this customer.
$AllAccountSKUs = Get-MsolAccountSku -TenantId $TenantID
$AllUserData = @()
$UnsupportedSKUUsers = @()

# Loop through the users and their licenses.
foreach ($O365User in $O365Users) {
    # Grab the user principal name and display name.
    $UserPrincipalName = $O365User.UserPrincipalName
    $DisplayName = $O365User.DisplayName

    # Determine whether this user already exists in CloudPlus
    $UserAlreadyExists = @($UserRecords | ? {$_.UserName.ToLower() -eq $UserPrincipalName.ToLower()}).Count -gt 0
    
    # Start building the list of the user's subscribed services in case they have a non-matching license.
    $UserSubscribedServices = @()
    $CurrentProducts = @()
    $RecommendedProduct = ""
    $RecommendedProductName = ""
    $RecommendedProductOfferID = ""
    $PercentMatch = 0
    
    # Loop through the user's O365 licenses to determine the recommended product to assign to the user.
    foreach ($LicensedProduct in $O365User.Licenses) {
        # Get the product ID for this SKU
        $LicenseSKUID = $LicensedProduct.AccountSkuId
        $CurrentSKU = $AllAccountSKUs | ? {$_.AccountSkuId -eq $LicenseSKUID}
        $LicenseSKUProduct = $CurrentSKU.SkuPartNumber
        $CurrentProducts += $LicenseSKUProduct
        
        # Loop through the ServiceStatus properties of this SKU to determine what components of Office 365 this user has access to
        foreach ($ServiceStatus in $CurrentSKU.ServiceStatus) {
            $UserSubscribedServices += $ServiceStatus.ServicePlan.ServiceName
        }

        # Determine if this user has any non-suppored SKUs
        if ($ValidO365SKUFeatures.Keys -notcontains $LicenseSKUProduct -and $UnsupportedSKUUsers -notcontains $UserPrincipalName) {
            $UnsupportedSKUUsers += $UserPrincipalName
        }
    }

    # Now that we've looped through the user's licenses we can make a determination on whether we need to make a recommendation for them within our own catalog.
    if ($CurrentProducts.Count -gt 0) {
        # Loop through the products to find one that is the cheapest one that matches all or most of the subscribed services belonging to the user.
        $UserServiceCount = $UserSubscribedServices.Count
        foreach ($Product in $ValidO365SKUFeatures.Keys) {
            $ProductFeatures = $ValidO365SKUFeatures[$Product]
            $ProductName = $ValidO365SKUNames[$Product]
            $ProductOfferID = $ValidO365SKUOfferIDs[$Product]
            $MatchingServiceCount = 0

            # Loop through the services belong to the user and check to see if it matches.
            foreach ($CurrentService in $UserSubscribedServices) {
                if ($ProductFeatures.Contains($CurrentService)) { $MatchingServiceCount++ }
            }

            # Calculate how much of a match this was.
            $ProductPercentageMatch = [math]::Round(([double]$MatchingServiceCount / [double]$UserServiceCount) * 100, 2)
            #ShowMessage "  Matching service count for product $Product : $ProductPercentageMatch%"

            # If this is more of a match than has previously been found, this is going to be the recommended product.
            if ($PercentMatch -lt $ProductPercentageMatch) {
                $PercentMatch = $ProductPercentageMatch
                $RecommendedProduct = $Product
                $RecommendedProductName = $ProductName
                $RecommendedProductOfferID = $ProductOfferID
            }

            # Break the loop if we've found a 100% match.
            if ($PercentMatch -eq 100) { break; }
        }
    } else {
        $PercentMatch = 100
    }

    # Compile the user data.
    $UserData = [ordered]@{
        TenantID = $TenantID
        UserPrincipalName = $UserPrincipalName
        DisplayName = $DisplayName
        ExistsInControlPanel = $UserAlreadyExists
        UnsupportedLicensesFound = $UnsupportedSKUUsers.Contains($UserPrincipalName)
        CurrentProducts = [string]::Join(", ", $CurrentProducts)
        RecommendedProduct = $RecommendedProduct
        RecommendedProductName = $RecommendedProductName
        RecommendedProductOfferID = $RecommendedProductOfferID
        ProductMatchPercentage = $PercentMatch
    }

    $AllUserData += New-Object PSObject -Property $UserData
}

$AllUserData | ConvertTo-Json