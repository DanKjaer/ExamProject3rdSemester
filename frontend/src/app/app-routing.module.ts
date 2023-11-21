import { NgModule } from '@angular/core';
import { PreloadAllModules, RouterModule, Routes } from '@angular/router';
import {AnimalBoxComponent} from "./animal-box/animal-box.component";
import { SpeciesComponent} from "./species/species.component"

const routes: Routes = [
  {
    path: '',
    component: AnimalBoxComponent
  },
  {
    path: 'species',
    component: SpeciesComponent
  }
];

@NgModule({
  imports: [
    RouterModule.forRoot(routes, { preloadingStrategy: PreloadAllModules })
  ],
  exports: [RouterModule]
})
export class AppRoutingModule { }
