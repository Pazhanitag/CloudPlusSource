param($AdminUsername,$AdminPassword,$Office365CustomerId,$Domain)

#####################################################################################################################################################################
function Get-CustomerDomainTxtRecord()
#####################################################################################################################################################################
{
    # Set up the credentials for the connection to Office 365. This should be the customer admin.
    $O365Credentials = New-Object -TypeName System.Management.Automation.PSCredential -ArgumentList $AdminUsername,(ConvertTo-SecureString -AsPlainText -Force -String $AdminPassword)
    
    # Connect to Office 365
    Connect-MsolService -Credential $O365Credentials

    # Run the command to get the TXT record Microsoft wants added to the account.
    $Info = Get-MsolDomainVerificationDns -DomainName $Domain -Mode DnsTXTRecord -TenantId $Office365CustomerId
    
    # Return the DNS TXT record we got back.
    $Info.Text
}

# Import the Microsoft Online module
Import-Module MSOnline

Get-CustomerDomainTxtRecord