param($AdminUsername,$AdminPassword,$Domain)

#####################################################################################################################################################################
function Federate-CustomerDomain()
#####################################################################################################################################################################
{
    # Set up the credentials for the connection to Office 365. This should be the customer admin.
    $O365Cred = New-Object -TypeName System.Management.Automation.PSCredential -ArgumentList $AdminUsername,(ConvertTo-SecureString -AsPlainText -Force -String $AdminPassword)
    
    # Connect to Office 365
    Connect-MsolService -Credential $O365Cred

    # Add the new domain to the account.
    Convert-MSOLDomainToFederated -DomainName $Domain -SupportMultipleDomain
}

# Import the Microsoft Online module
Import-Module MSOnline

Federate-CustomerDomain