param($upn, $newPassword)
#########################################################################################################################################
function ChangePassword() 
#########################################################################################################################################
{
   $idenntity = Get-ADUser -Filter {UserPrincipalName -eq $upn}
   Set-ADAccountPassword -Identity $idenntity -Reset -NewPassword (ConvertTo-SecureString $newPassword -Force -AsPlainText)
}

ChangePassword
