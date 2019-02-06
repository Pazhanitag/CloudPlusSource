param($AdminUsername,$AdminPassword,$CustomerO365Domain,$SecurityGroupName)

#####################################################################################################################################################################
function Add-SecurityGroup()
#####################################################################################################################################################################
{
    
    # Set up the credentials for the connection to Office 365. This should be the customer admin.
    $O365Cred = New-Object -TypeName System.Management.Automation.PSCredential -ArgumentList $AdminUsername,(ConvertTo-SecureString -AsPlainText -Force -String $AdminPassword)

    # Connect to Office 365
    Connect-MsolService -Credential $O365Cred
    
    # Retrieve the Office 365 Tenant ID
    $tenID=(Get-MSOLPartnerContract -Domain $CustomerO365Domain).tenantId.guid
	
    # Add the new security group to the tenant.
    New-MsolGroup -Name $SecurityGroupName -tenantId $tenID
}

# Import the Microsoft Online module
Import-Module MSOnline

Add-SecurityGroup