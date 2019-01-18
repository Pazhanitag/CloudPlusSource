param($samAccountName)

#########################################################################################################################################
function SamAccountNameAvailable() 
#########################################################################################################################################
{
    Try {
        $idrefUser = ([System.Security.Principal.NTAccount]($samAccountName)).Translate([System.Security.Principal.SecurityIdentifier])
    }
    catch [System.Security.Principal.IdentityNotMappedException] {
        $idrefUser = $null
    }
           
    If ($idrefUser) {
        return $false
    }
    Else {
        return $true
    }
}
SamAccountNameAvailable
