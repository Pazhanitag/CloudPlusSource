param($AdminUsername,$AdminPassword,$Office365CustomerId,$Upn)

#####################################################################################################################################################################
function Set-CustomerImmutableID()
#####################################################################################################################################################################
{
    # Set up the credentials for the connection to Office 365. This should be the customer admin.
    $O365Cred = New-Object -TypeName System.Management.Automation.PSCredential -ArgumentList $AdminUsername,(ConvertTo-SecureString -AsPlainText -Force -String $AdminPassword)
    
    # Connect to Office 365
    Connect-MsolService -Credential $O365Cred

    # Retrieve the Office 365 Tenant ID
    #$tenID=(Get-MSOLPartnerContract -Domain $O365PrimaryDomain).tenantId.guid

    # Get the GUID for on-premise user that correlates to the Msol User
    $guid = (get-aduser -f {UserPrincipalName -eq $Upn} -pr objectguid).objectguid
    
    # Convert the GUID to a Base64 value for input into the ImmutableID field.
    $ImmutableID = [System.Convert]::ToBase64String($guid.ToByteArray())

    #Set the Immutable ID attribute on the MSOL User object.
    Set-MsolUser -userprincipalname $Upn -immutableID $ImmutableID -TenantId $Office365CustomerId
}

# Import the Microsoft Online module
Import-Module MSOnline

Set-CustomerImmutableID