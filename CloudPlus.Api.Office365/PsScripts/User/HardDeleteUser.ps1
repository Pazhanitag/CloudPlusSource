param($AdminUsername,$AdminPassword,$Office365CustomerId,$UserPrincipalName)

#####################################################################################################################################################################
function Hard-Delete-User()
#####################################################################################################################################################################
{
    # Set up the credentials for the connection to Office 365. This should be the customer admin.
    $O365Cred = New-Object -TypeName System.Management.Automation.PSCredential -ArgumentList $AdminUsername,(ConvertTo-SecureString -AsPlainText -Force -String $AdminPassword)
    
    # Connect to Office 365
    Connect-MsolService -Credential $O365Cred

    #Hard delete the user from the recycle bin
    Remove-MsolUser -UserPrincipalName $UserPrincipalName -Force -tenantId $Office365CustomerId
    Remove-MsolUser -UserPrincipalName $UserPrincipalName -Force -tenantId $Office365CustomerId -RemoveFromRecycleBin  

}

# Import the Microsoft Online module
Import-Module MSOnline

Hard-Delete-User