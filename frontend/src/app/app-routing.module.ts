import { NgModule } from '@angular/core';
import { PreloadAllModules, RouterModule, Routes } from '@angular/router';
import {AnimalBoxComponent} from "./animal-box/animal-box.component";
import { SpeciesComponent} from "./species/species.component"
import {AnimalsComponent} from "./animals/animals.component";
import {State} from "../state";
import {LoginComponent} from "./login/login.component";
import {EmployeeComponent} from "./employee/employee.component";


const routes: Routes = [
  {
    path: '',
    component: AnimalBoxComponent
  },
  {
    path: 'species/:id',
    component: SpeciesComponent
  },
  {
    path: 'animals/:id',
    component: AnimalsComponent
  },
  {
    path: 'login',
    component: LoginComponent,
  },
  {
    path: 'staff',
    component: EmployeeComponent
  }
];

@NgModule({
  imports: [
    RouterModule.forRoot(routes, { preloadingStrategy: PreloadAllModules })
  ],
  exports: [RouterModule]
})
export class AppRoutingModule { }
