
import { catchError, throwError } from 'rxjs';
import { ToastrService } from 'ngx-toastr';
import { inject } from '@angular/core';
import { HttpInterceptorFn } from '@angular/common/http';
import { environment } from 'src/environments/environment';

export const JwtInterceptor: HttpInterceptorFn = (req, next) => 
{
  let toastrServ = inject(ToastrService);

  if(environment.infoJoueur)
  {
    req = req.clone({
      headers: req.headers.set("Authorization", `Bearer ${environment.infoJoueur.Jwt}`)
    });
  }

  return next(req).pipe(
    catchError(
      (erreur) =>
      {
        console.log(erreur);

        switch (erreur.status) 
        {
          case 500:
            toastrServ.warning("Erreur interne c'est produite");
            break;

          case 404:
          case 400:
            toastrServ.error(erreur.error);
            break;
        
          default:
            toastrServ.error("Erreur pas de réseau");
            break;
        }
          
        return throwError(() => null);
      }
    )
  );
};
