param($targetDomainController, $baseHostingOu, $customerAccountId)
#########################################################################################################################################
function CreateCompany() 
#########################################################################################################################################
{
   New-ADOrganizationalUnit -Server $targetDomainController -Path "$baseHostingOu" -Name $customerAccountId

   New-ADGroup -Server $targetDomainController -Path $customerOu -Name $customerAllUsersGroup -GroupScope Universal -GroupCategory Security -DisplayName $customerAllUsersGroup -Description $customerAccountId
   New-ADGroup -Server $targetDomainController -Path $customerOu -Name $customerAdminUsersGroup -GroupScope Universal -GroupCategory Security -DisplayName $customerAdminUsersGroup -Description $customerAccountId
}

$customerAllUsersGroup = "AllUsers@" + $customerAccountId
$customerAdminUsersGroup = "Admins@" + $customerAccountId
$customerOu = "OU=" + $customerAccountId + "," + $baseHostingOu
CreateCompany
