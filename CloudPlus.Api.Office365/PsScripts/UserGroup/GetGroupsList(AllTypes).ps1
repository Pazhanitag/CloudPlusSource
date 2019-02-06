param($AdminUsername,$AdminPassword,$Office365CustomerId)

#####################################################################################################################################################################
function Get-Groups()
#####################################################################################################################################################################
{
    # Set up the credentials for the connection to Office 365. This should be the customer admin.
    $O365Cred = New-Object -TypeName System.Management.Automation.PSCredential -ArgumentList $AdminUsername,(ConvertTo-SecureString -AsPlainText -Force -String $AdminPassword)
   
    # Connect to Office 365
    Connect-MsolService -Credential $O365Cred
 
    $groups = Get-MsolGroup -TenantId $Office365CustomerId
 
}
 
# Import the Microsoft Online module
Import-Module MSOnline
 
Get-Groups