param($AdminUsername,$AdminPassword,$CustomerO365Domain,$SecurityGroupName,$UserSMTPAddress)

#####################################################################################################################################################################
function Add-SecurityGroupMember()
#####################################################################################################################################################################
{
    
    # Set up the credentials for the connection to Office 365. This should be the customer admin.
    $O365Cred = New-Object -TypeName System.Management.Automation.PSCredential -ArgumentList $AdminUsername,(ConvertTo-SecureString -AsPlainText -Force -String $AdminPassword)

    # Connect to Office 365
    Connect-MsolService -Credential $O365Cred
    
    # Retrieve the Office 365 Tenant ID
    $tenID=(Get-MSOLPartnerContract -Domain $CustomerO365Domain).tenantId.guid
	
    # Get the GUID for the security group to be modified.
    $SecurityGroup = Get-MsolGroup -Name $SecurityGroupName -tenantId $tenID
	$SecurityGroupGUID = $SecurityGroup.ObjectId
	
	# Get the GUID of the user to be added to the group
	$User = Get-MsolUser -UserPrincipalName $UserSMTPAddress
	$UserGUID = $User.ObjectId
	
	# Add the User to the security group
	Add-MsolGroupMember -GroupObjectId $SecurityGroupGUID -GroupMemberType $User -GroupMemberObjectId $UserGUID -TenantID $tenID
}

# Import the Microsoft Online module
Import-Module MSOnline

Add-SecurityGroupMember