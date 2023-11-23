import { Component, OnInit } from '@angular/core';
import {state} from "@angular/animations";
import {State} from "../../state";
import {firstValueFrom} from "rxjs";
import {AnimalFeed} from "../../models";
import {HttpClient} from "@angular/common/http";
import {Router} from "@angular/router";

@Component({
  selector: 'app-animal-names',
  templateUrl: './animal-names.component.html',
  styleUrls: ['./animal-names.component.scss'],
})
export class AnimalNamesComponent  implements OnInit {

  constructor(public state: State, public http: HttpClient, public router: Router) { }

  ngOnInit() {
    this.getAnimalFeed(<number>this.state.currentAnimalSpecies.speciesID);
  }

  async getAnimalFeed(speciesId: number){
    try{
      const result = await firstValueFrom(this.http.get<AnimalFeed[]>('http://localhost:5000/api/animalfeed/' + speciesId));
      console.log(result);
      this.state.animalFeed = result!;
    }catch(error){
      console.error('Error fetching data:', error)
    }
  }

  goToAnimal(animalId: number) {
    this.router.navigate(['/animals/' + animalId])
  }
}
