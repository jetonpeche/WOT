import { CanActivateFn } from '@angular/router';
import { environment } from 'src/environments/environment';

export const StrateurGuard: CanActivateFn = (route, state) =>
{
  if(environment.infoJoueur?.EstStrateur || environment.infoJoueur?.EstAdmin)
    return true;

  return false;
};
