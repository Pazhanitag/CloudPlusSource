param($AdminUsername,$AdminPassword,$CustomerO365Domain,$SecurityGroupName)

#####################################################################################################################################################################
function Remove-SecurityGroup()
#####################################################################################################################################################################
{
    
    # Set up the credentials for the connection to Office 365. This should be the customer admin.
    $O365Cred = New-Object -TypeName System.Management.Automation.PSCredential -ArgumentList $AdminUsername,(ConvertTo-SecureString -AsPlainText -Force -String $AdminPassword)

    # Connect to Office 365
    Connect-MsolService -Credential $O365Cred

	# Retrieve the Office 365 Tenant ID
    $tenID=(Get-MSOLPartnerContract -Domain $CustomerO365Domain).tenantId.guid
		
    # Remove the new security group from the tenant.
    $GroupId = Get-MsolGroup -SearchString $SecurityGroupName -tenantId $tenID
	Remove-MsolGroup -objectid $GroupId -tenantId $tenID -force
}

# Import the Microsoft Online module
Import-Module MSOnline

Add-SecurityGroup