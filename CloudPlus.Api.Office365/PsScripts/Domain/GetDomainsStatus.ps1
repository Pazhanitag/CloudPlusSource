<#
.SYNOPSIS

This script is used to perform a check on an O365 tenant to determine what their recommended product is and whether it already exists
as a user in the Control Panel database. It is intended for the development team to take this information as the basis for an end-user
page which would allow a user who is importing an O365 tenant to see what work needs to be done and what options they want to set for
the users that need to be created.

.DESCRIPTION


.PARAMETER CustomerDomain

Any domain belong to the O365 tenant and CP customer.
#>
param($AdminUsername,$AdminPassword,$SQLServerIP,$SQLServerUsername,$SQLServerPassword,$AuthDBName,$CPDBName,$Office365CustomerId,$Domain)

##################################################
# Configurable values
##################################################
#TODO
#$ScriptPath = Split-Path $MyInvocation.MyCommand.Path -Parent

#TODO
#$AuthDBConnectionString = "server=192.168.73.156;initial catalog=CloudPlusAuth;trusted_connection=yes;MultipleActiveResultSets=true;App=EntityFramework"
#$CPDBConnectionString = "data source=192.168.73.156;initial catalog=CloudPlusDb;integrated security=True;MultipleActiveResultSets=True;App=EntityFramework"
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
function Query-CPDatabase {
    Param(
        [string]$Query
        )

    # Get the data from the CP database
    $SQLCommand_Current = New-Object System.Data.SqlClient.SqlCommand
    $SQLCommand_Current.Connection = $CPDatabaseConnection
    $SQLCommand_Current.CommandText = $Query
    $SQLReader_Current = $SQLCommand_Current.ExecuteReader()

    $ReturnData = New-Object System.Data.DataTable
    $ReturnData.Load($SQLReader_Current)

    $SQLCommand_Current.Dispose()

    return $ReturnData
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

# Connect to the Office 365 service.
$O365Cred = New-Object -TypeName System.Management.Automation.PSCredential -ArgumentList $AdminUsername,(ConvertTo-SecureString -AsPlainText -Force -String $AdminPassword)
try {
    Import-Module MSOnline

    Connect-MsolService -Credential $O365Cred -ErrorAction Stop
} catch {
    throw "Unable to authenticate to Office 365 Service with error `"$($_.Exception.Message)`"."
}

# Obtain the list of the domains belonging to this customer.
$O365Domains = @(Get-MsolDomain -TenantId $TenantID)

# Get the list of domains already in this customer.
$DomainRecords = Query-CPDatabase -Query "SELECT [Name] FROM Domains WHERE Company_Id = $CustomerID AND IsDeleted = 0"

# Get the list of all domains in the control panel. This is used to identify customer domains that are in the CP but may belong to another customer.
$AllDomainRecords = Query-CPDatabase -Query "SELECT [Name] FROM Domains WHERE IsDeleted = 0"

$AllDomainData = @()

# Loop through the domains.
foreach ($O365Domain in $O365Domains) {
    # Grab the domain name, DNS verification status, and authentication status
    $O365DomainName = $O365Domain.Name
    $O365DomainVerified = ($O365Domain.Status -eq 'Verified')
    $O365DomainFederated = ($O365Domain.Authentication -eq 'Federated')

    # Determine whether this domain already exists in this CloudPlus customer.
    $DomainAlreadyExists = @($DomainRecords | ? {$_.Name.ToLower() -eq $O365DomainName.ToLower()}).Count -gt 0

    # Determine whether this domain already exists but in another customer.
    $DomainExistsInAnotherCustomer = ($DomainAlreadyExists -eq $false -and @($AllDomainRecords | ? {$_.Name.ToLower() -eq $O365DomainName.ToLower()}).Count -gt 0)
    
    # Compile the user data.
    $DomainData = [ordered]@{
        TenantID = $TenantID
        DomainName = $O365DomainName
        DomainVerified = $O365DomainVerified
        DomainFederated = $O365DomainFederated
        ExistsInCustomer = $DomainAlreadyExists
        ExistsInDifferentCustomer = $DomainExistsInAnotherCustomer        
    }

    $AllDomainData += New-Object PSObject -Property $DomainData
}

$AllDomainData | ConvertTo-Json