
param($AdminUsername,$AdminPassword,$Office365CustomerId,$UserPrincipalName,$DisplayName,$FirstName,$LastName,$UsageLocation,
$City,$Country,$PhoneNumber,$PostalCode,$State,$StreetAddress, $Password)

#####################################################################################################################################################################
function Create-User()
#####################################################################################################################################################################
{
    # Set up the credentials for the connection to Office 365. This should be the customer admin.
    $O365Cred = New-Object -TypeName System.Management.Automation.PSCredential -ArgumentList $AdminUsername,(ConvertTo-SecureString -AsPlainText -Force -String $AdminPassword)
    
    # Connect to Office 365
    Connect-MsolService -Credential $O365Cred

    # Retrieve the Office 365 Tenant ID
    #$tenID=(Get-MSOLPartnerContract -Domain $O365PrimaryDomain).tenantId.guid
	$User = Get-MsolUser -UserPrincipalName $UserPrincipalName -TenantId $Office365CustomerId -ErrorAction SilentlyContinue
        
    If ($User -ne $Null) {
        return $User.ObjectId
        break
    }

    # Get the GUID for on-premise user that correlates to the Msol User
    $guid = (get-aduser -f {UserPrincipalName -eq $UserPrincipalName} -pr objectguid).objectguid
    
	if([string]::IsNullOrEmpty($guid)) {
		throw "No user in AD $UserPrincipalName"
	}

    # Convert the GUID to a Base64 value for input into the ImmutableID field.
    $ImmutableID = [System.Convert]::ToBase64String($guid.ToByteArray())

	(New-MsolUser -UserPrincipalName $UserPrincipalName `
	-ImmutableId $ImmutableID `
	-TenantId $Office365CustomerId `
	-DisplayName $DisplayName `
	-FirstName $FirstName `
	-LastName $LastName `
	-UsageLocation $UsageLocation `
	-ForceChangePassword $FALSE `
	-Password $Password `
	-City $City `
	-Country $Country `
	-PhoneNumber $PhoneNumber `
	-PostalCode $PostalCode `
	-State $State `
	-StreetAddress $StreetAddress).ObjectId
}

# Import the Microsoft Online module
Import-Module MSOnline

Create-User