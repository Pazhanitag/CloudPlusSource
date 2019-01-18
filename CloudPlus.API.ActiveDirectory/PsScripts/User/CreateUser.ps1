param($displayName, $upn, $samAccountName, $password, $customerAccountId,$customerPrimaryDomain, 
	$baseHostingOu, $targetDomainController, $givenName, $surName, $phone, $address, $city, $state, $zip, $country,
	$title, $company, $emailAddress)

$customerOu = "OU=" + $customerAccountId +"," + $baseHostingOu
$identity = "CN="+ $upn + "," + $customerOu 
$customerAllUsersGroupCn = "CN=AllUsers@" + $customerAccountId + "," + $customerOu 

#########################################################################################################################################
function CreateGeneralUser() 
#########################################################################################################################################
{
    $userSecurePassword = $password | ConvertTo-SecureString -AsPlainText -Force
    $customerOu = "OU=" + $customerAccountId +"," + $baseHostingOu
    New-ADUser -Server $targetDomainController `
    -Path $customerOu `
    -Name $upn `
    -DisplayName $displayName `
    -ChangePasswordAtLogon:$false `
    -Enabled:$true `
    -AccountPassword $userSecurePassword `
    -UserPrincipalName $upn `
    -SamAccountName $samAccountName `
    -Description "$customerAccountId - User" `
    -PasswordNeverExpires:$true -PasswordNotRequired:$true `
	-GivenName $givenName `
    -Surname $surName `
    -MobilePhone $phone `
    -HomePhone $phone `
	-OfficePhone $phone `
    -StreetAddress $address `
    -City $city `
    -State $state `
    -PostalCode $zip `
    -Country $country `
	-Title $title `
	-Company $company `
	-EmailAddress $emailAddress `
    -OtherAttributes @{extensionAttribute10=$customerAccountId} | Out-Null
	
     $groupidentity=$identity
	# Removed since the UPN should not contain commas
    #if($displayName.Contains(","))
    #   {
    #        $index=$displayName.IndexOf(',')
    #        
    #        $groupidentity = "CN="+ $displayName.Insert($index,"\") + "," + $customerOu 
    #   }
    Add-ADGroupMember -Server $targetDomainController -Identity $customerAllUsersGroupCn -Members $groupidentity
}
Import-Module ActiveDirectory

CreateGeneralUser
