import { Injectable } from '@angular/core';
import {
  HttpRequest,
  HttpHandler,
  HttpEvent,
  HttpInterceptor
} from '@angular/common/http';
import { catchError, Observable, throwError } from 'rxjs';
import { ToastrService } from 'ngx-toastr';

@Injectable()
export class JwtInterceptor implements HttpInterceptor {

  constructor(private toastrServ: ToastrService) {}

  intercept(request: HttpRequest<unknown>, next: HttpHandler): Observable<HttpEvent<unknown>> {
    return next.handle(request).pipe(
      catchError(
        (erreur) =>
        {
          if(erreur.status == 500)
            this.toastrServ.warning("Erreur interne c'est produite");
          else if(erreur.status == 0)
            this.toastrServ.error("Erreur pas de réseau");

          return throwError(() => null);
        }
      )
    );
  }
}
