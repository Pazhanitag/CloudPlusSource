<#
.SYNOPSIS

This script is used to check whether a specified customer domain can be found within a tenant of our Office 365 Partner Center. Returns the tenant ID guid if found or "False" if not found.

.DESCRIPTION

Locate the domain within Office 365. Returns the tenant ID guid if found or "False" if not found.

.PARAMETER Domain

Any domain belong to the customer you wish to locate.
#>
param($AdminUsername,$AdminPassword,$Domain)

##################################################
# MAIN SCRIPT LOGIC
##################################################
# Connect to the Office 365 service.
$O365Cred = New-Object -TypeName System.Management.Automation.PSCredential -ArgumentList $AdminUsername,(ConvertTo-SecureString -AsPlainText -Force -String $AdminPassword)
try {
    Import-Module MSOnline

    Connect-MsolService -Credential $O365Cred -ErrorAction Stop
} catch {
    Write-Host "Unable to authenticate to Office 365 Service with error `"$($_.Exception.Message)`"."
    Exit
}

##################################################
# Locate domain within O365
##################################################
# Attempt to find the customer quickly using Office 365 domain name check
$Office365Tenant = Get-MsolPartnerContract -DomainName $Domain
$CustomerTenantFound = $false
if ($Office365Tenant) {
    $CustomerTenantFound = $true
}

# If the customer wasn't found, crawl all customers to look for the domain.
if ($CustomerTenantFound -eq $false) {
    $AllTenants = Get-MsolPartnerContract
    foreach ($Tenant in $AllTenants) {
        $TenantDomains = Get-MsolDomain -TenantId $Tenant.TenantId -ErrorAction SilentlyContinue
        foreach ($TenantDomain in $TenantDomains) {
            if ($TenantDomain.Name.ToLower() -eq $Domain.ToLower()) {
                $Office365Tenant = $Tenant
                $CustomerTenantFound = $true
                break;
            }
        }
    }
}

if ($CustomerTenantFound -eq $false) {
    return "False"
}
return $Office365Tenant.TenantId.ToString()