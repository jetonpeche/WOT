import { importProvidersFrom } from '@angular/core';
import { AppComponent } from './app/app.component';
import { MatListModule } from '@angular/material/list';
import { MatTooltipModule } from '@angular/material/tooltip';
import { MatPaginatorModule } from '@angular/material/paginator';
import { MatSortModule } from '@angular/material/sort';
import { MatTableModule } from '@angular/material/table';
import { MatRadioModule } from '@angular/material/radio';
import { MatCheckboxModule } from '@angular/material/checkbox';
import { MatSelectModule } from '@angular/material/select';
import { MatIconModule } from '@angular/material/icon';
import { MatToolbarModule } from '@angular/material/toolbar';
import { MatDialogModule } from '@angular/material/dialog';
import { MatProgressSpinnerModule } from '@angular/material/progress-spinner';
import { MatInputModule } from '@angular/material/input';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatButtonModule } from '@angular/material/button';
import { MatCardModule } from '@angular/material/card';
import { ToastrModule } from 'ngx-toastr';
import { provideAnimations } from '@angular/platform-browser/animations';
import { routes } from './app/app-routing.module';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { BrowserModule, bootstrapApplication } from '@angular/platform-browser';
import { provideHttpClient, withInterceptors } from '@angular/common/http';
import { LocationStrategy, HashLocationStrategy } from '@angular/common';
import { JoueurService } from './app/service/joueur.service';
import { OutilService } from './app/service/outil.service';
import { TankService } from './app/service/tank.service';
import { ClanWarService } from './app/service/clan-war.service';
import { JwtInterceptor } from './app/interceptor/jwt.interceptor';
import { provideRouter } from '@angular/router';

bootstrapApplication(AppComponent, {
    providers: [
        importProvidersFrom(BrowserModule, FormsModule, ReactiveFormsModule, ToastrModule.forRoot({
            timeOut: 3000,
            progressBar: true,
            progressAnimation: 'increasing'
            //positionClass: 
        }), MatCardModule, MatButtonModule, MatFormFieldModule, MatInputModule, MatProgressSpinnerModule, MatDialogModule, MatToolbarModule, MatIconModule, MatSelectModule, MatCheckboxModule, MatRadioModule, MatTableModule, MatSortModule, MatPaginatorModule, MatTooltipModule, MatListModule),
        { provide: LocationStrategy, useClass: HashLocationStrategy },

        provideHttpClient(withInterceptors([JwtInterceptor])),
        provideAnimations(),
        provideRouter(routes),

        { provide: JoueurService, useClass: JoueurService },
        { provide: OutilService, useClass: OutilService },
        { provide: TankService, useClass: TankService },
        { provide: ClanWarService, useClass: ClanWarService },
    ]
})
  .catch(err => console.error(err));
