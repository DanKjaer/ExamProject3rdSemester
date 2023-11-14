import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';
import { RouteReuseStrategy } from '@angular/router';

import { IonicModule, IonicRouteStrategy } from '@ionic/angular';

import { AppComponent } from './app.component';
import { AppRoutingModule } from './app-routing.module';
import {CustomToolbarComponent} from "./custom-toolbar/custom-toolbar.component";
import {AnimalBoxComponent} from "./animal-box/animal-box.component";
import {ProfilePictureComponent} from "./profile-picture/profile-picture.component";
import {BrowserAnimationsModule} from "@angular/platform-browser/animations";

@NgModule({
    declarations: [AppComponent, CustomToolbarComponent, AnimalBoxComponent, ProfilePictureComponent],
    imports: [BrowserModule, IonicModule.forRoot(), AppRoutingModule, BrowserAnimationsModule],

  providers: [{ provide: RouteReuseStrategy, useClass: IonicRouteStrategy }],
  bootstrap: [AppComponent],
})
export class AppModule {}
