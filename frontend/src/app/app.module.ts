import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';
import {RouteReuseStrategy, RouterModule} from '@angular/router';

import { IonicModule, IonicRouteStrategy } from '@ionic/angular';

import { AppComponent } from './app.component';
import { AppRoutingModule } from './app-routing.module';
import {CustomToolbarComponent} from "./custom-toolbar/custom-toolbar.component";
import {AnimalBoxComponent} from "./animal-box/animal-box.component";
import {ProfilePictureComponent} from "./profile-picture/profile-picture.component";
import {BrowserAnimationsModule} from "@angular/platform-browser/animations";
import {HTTP_INTERCEPTORS, HttpClientModule} from "@angular/common/http";
import {SpeciesComponent} from "./species/species.component";
import {AnimalPictureComponent} from "./animal-picture/animal-picture.component";
import {AnimalNamesComponent} from "./animal-names/animal-names.component";
import {AnimalInformationComponent} from "./animal-information/animal-information.component";
import {AnimalsComponent} from "./animals/animals.component";
import {LoginComponent} from "./login/login.component";
import {ErrorHttpInterceptor} from "../interceptors/error-http-interceptors";

@NgModule({
  declarations: [AppComponent, CustomToolbarComponent, AnimalBoxComponent, ProfilePictureComponent,
    SpeciesComponent, AnimalPictureComponent, AnimalNamesComponent, AnimalInformationComponent, AnimalsComponent, LoginComponent],
    imports: [BrowserModule, IonicModule.forRoot(), AppRoutingModule, BrowserAnimationsModule, HttpClientModule, AppRoutingModule, RouterModule],
    exports: [RouterModule, AnimalBoxComponent],
  providers: [{ provide: RouteReuseStrategy, useClass: IonicRouteStrategy }, {provide: HTTP_INTERCEPTORS, useClass: ErrorHttpInterceptor, multi: true }],
  bootstrap: [AppComponent],
})
export class AppModule {}
