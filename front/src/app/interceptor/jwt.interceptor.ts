
import { catchError, throwError } from 'rxjs';
import { ToastrService } from 'ngx-toastr';
import { inject } from '@angular/core';
import { HttpInterceptorFn } from '@angular/common/http';

export const JwtInterceptor: HttpInterceptorFn = (req, next) => 
{
  let toastrServ = inject(ToastrService);

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
        
          default:
            toastrServ.error("Erreur pas de réseau");
            break;
        }
          
        return throwError(() => null);
      }
    )
  );
};
