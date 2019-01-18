param($displayName, $upn, $givenName, $surName, $phone, $address, $city, $state, $zip, $country,
	$title, $company, $emailAddress)

#########################################################################################################################################
function UpdateGeneralUser() 
#########################################################################################################################################
{
	$idenntity = Get-ADUser -Filter {UserPrincipalName -eq $upn}
    $RenameObject = $false
	# Removed 10/9/2018 since user CN should be UPN-based now.
    #if ($idenntity.DisplayName -ne $displayName) {
    #    $RenameObject = $true
    #}
    Set-ADUser -Identity $idenntity `
    -DisplayName $displayName `
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
	-EmailAddress $emailAddress

	# Removed 10/9/2018 since user CN should be UPN-based now.
    #if ($RenameObject -eq $true) {
    #    Rename-ADObject $idenntity -NewName $displayName
    #}
}
Import-Module ActiveDirectory

UpdateGeneralUser
