param($AdminUsername,$AdminPassword,$UserPrincipalName,$Roles,$Office365CustomerId)

#####################################################################################################################################################################
function Assign-User-Roles()
#####################################################################################################################################################################
{
    # Set up the credentials for the connection to Office 365. This should be the customer admin.
    $O365Cred = New-Object -TypeName System.Management.Automation.PSCredential -ArgumentList $AdminUsername,(ConvertTo-SecureString -AsPlainText -Force -String $AdminPassword)
    
    # Connect to Office 365
    Connect-MsolService -Credential $O365Cred

    # Add the user to the role group. Available roles include: 
	#  - User Account Administrator, 
	#  - Lync Service Administrator, 
    #  - Exchange Service Administrator, 
	#  - SharePoint Service Administrator, 
	#  - CRM Service Administrator and 
	#  - Service Support Administrator.
    # Please note, the Lync Administrator listed as "Skype for Business Adminisitrator" in the management portal. It is also 
    # recommended that the Service Administrator be assigned to anyone holding the Exchange, Lync, CRM or SharePoint administrator roles.

	$Roles | ForEach-Object	{
		try {
				Remove-MsolRoleMember -RoleMemberEmailAddress $UserPrincipalName -RoleName $_ -TenantId $Office365CustomerId
				Add-MsolRoleMember -RoleMemberEmailAddress $UserPrincipalName -RoleName $_ -TenantId $Office365CustomerId
			}
		catch {

		}
	}
}

# Import the Microsoft Online module
Import-Module MSOnline

Assign-User-Roles