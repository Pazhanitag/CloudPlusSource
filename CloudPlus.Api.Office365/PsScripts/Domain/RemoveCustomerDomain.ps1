param($AdminUsername,$AdminPassword,$Office365CustomerId,$Domain)

#####################################################################################################################################################################
function Remove-CustomerDomain()
#####################################################################################################################################################################
{
    # Set up the credentials for the connection to Office 365. This should be the customer admin.
    $O365Cred = New-Object -TypeName System.Management.Automation.PSCredential -ArgumentList $AdminUsername,(ConvertTo-SecureString -AsPlainText -Force -String $AdminPassword)
    
    # Connect to Office 365
    Connect-MsolService -Credential $O365Cred

    # Run the command to remove the domain from the account. The -Force is to skip the confirmation prompt.
    Remove-MsolDomain -DomainName $Domain -Force -tenantId $Office365CustomerId
}

# Import the Microsoft Online module
Import-Module MSOnline

Remove-CustomerDomain