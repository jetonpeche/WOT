import { CanActivateFn } from '@angular/router';
import { environment } from 'src/environments/environment';

export const AdminGuard: CanActivateFn = (route, state) => 
{
  if(environment.infoJoueur?.EstAdmin)
    return true;

  return false;
};
