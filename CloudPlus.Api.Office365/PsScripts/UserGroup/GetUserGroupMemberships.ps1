param($AdminUsername,$AdminPassword,$UserName,$CustomerO365Domain)

#####################################################################################################################################################################
function Get-UserGroupMemberships()
#####################################################################################################################################################################
{
    
    # Set up the credentials for the connection to Office 365. This should be the customer admin.
    $O365Cred = New-Object -TypeName System.Management.Automation.PSCredential -ArgumentList $AdminUsername,(ConvertTo-SecureString -AsPlainText -Force -String $AdminPassword)

    # Connect to Office 365
    Connect-MsolService -Credential $O365Cred

	# Retrieve the Office 365 Tenant ID
    $tenID=(Get-MSOLPartnerContract -Domain $CustomerO365Domain).tenantId.guid
    
    # Get UserName (THIS DOES NOT WORK - it was an experiment)
	$User = Get-MSOLUser -UserPrincipalName $UserName -TenantID $tenID

	# List Groups of which the user is a member
    $Groups = Get-MsolGroup -TenantID $tenID -All
    $MemberOfGroups = @()

    foreach ($CurrentGroup in $Groups) {
        $GroupMemberships = Get-MsolGroupMember -TenantId $tenID -GroupObjectId $CurrentGroup.ObjectId -MaxResults 1000
        if ($GroupMemberships.ObjectId -contains $User.ObjectId) {
            $MemberOfGroups += [PSCustomObject]@{
                GroupId = $CurrentGroup.ObjectId
                GroupName = $CurrentGroup.DisplayName
            }
        }
    }

    @{ GroupMemberships = @($MemberOfGroups) } | ConvertTo-Json
}
# Import the Microsoft Online module
Import-Module MSOnline

Get-UserGroupMemberships