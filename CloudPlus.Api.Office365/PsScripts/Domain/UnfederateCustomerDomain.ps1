param($AdminUsername,$AdminPassword,$Domain,$Office365CustomerId)

﻿#####################################################################################################################################################################
function Set-DomainToStandard()
#####################################################################################################################################################################
{
    # Set up the credentials for the connection to Office 365. This should be the customer admin.
    $O365Cred = New-Object -TypeName System.Management.Automation.PSCredential -ArgumentList $AdminUsername,(ConvertTo-SecureString -AsPlainText -Force -String $AdminPassword)

    # Connect to Office 365
    Connect-MsolService -Credential $O365Cred
    
    # Convert the domain from federated to standard.
    Convert-MsolDomainToStandard -DomainName $Domain -SkipUserConversion $true –PasswordFile "C:\$($Domain)_Passwords.txt"

    # Get the list of users and convert them to standard.
    $AllO365Users = Get-MsolUser -TenantId $Office365CustomerId | ? {$_.UserPrincipalName -like "*@$Domain"}
    foreach ($CurrentUser in $AllO365Users) {
        $NewPassword = Get-RandomPassword -length 10 -sourcedata $ascii
        Convert-MsolFederatedUser -UserPrincipalName $CurrentUser.UserPrincipalName -TenantId $Office365CustomerId -NewPassword $NewPassword
        [PSCustomObject]@{UserPrincipalName=$($CurrentUser.UserPrincipalName);Password=$NewPassword} | Export-CSV "C:\$($Domain)_Passwords.csv" -Append -NoTypeInformation
    }
}

# Import the Microsoft Online module
Import-Module MSOnline

Set-DomainToStandard