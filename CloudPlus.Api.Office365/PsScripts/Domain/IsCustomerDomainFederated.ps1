param($AdminUsername,$AdminPassword,$Office365CustomerId,$Domain)

#####################################################################################################################################################################
function Is-CustomerDomain-Federated()
#####################################################################################################################################################################
{
    
    # Set up the credentials for the connection to Office 365. This should be the customer admin.
    $O365Cred = New-Object -TypeName System.Management.Automation.PSCredential -ArgumentList $AdminUsername,(ConvertTo-SecureString -AsPlainText -Force -String $AdminPassword)

    # Connect to Office 365
    Connect-MsolService -Credential $O365Cred
    
    # Add the new domain to the account.
    $msolDomain = Get-MsolDomain -DomainName $Domain -TenantId $Office365CustomerId

	$msolDomain.authentication -eq "Federated"
}

# Import the Microsoft Online module
Import-Module MSOnline

Is-CustomerDomain-Federated