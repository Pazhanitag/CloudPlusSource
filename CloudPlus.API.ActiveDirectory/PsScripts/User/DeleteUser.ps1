﻿param($upn)
#########################################################################################################################################
function DeleteUser() 
#########################################################################################################################################
{
   $idenntity = Get-ADUser -Filter {UserPrincipalName -eq $upn}
   Remove-ADUser -Identity $idenntity -Confirm:$false
}

DeleteUser