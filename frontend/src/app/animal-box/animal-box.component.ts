import { Component, OnInit } from '@angular/core';
import {HttpClient} from "@angular/common/http";
import {AnimalSpeciesFeed} from "../../models";
import {firstValueFrom} from "rxjs";
import {State} from "../../state";
import {Router} from "@angular/router";


@Component({
  selector: 'app-animal-box',
  templateUrl: './animal-box.component.html',
  styleUrls: ['./animal-box.component.scss'],

})
export class AnimalBoxComponent  implements OnInit {

  constructor(public http: HttpClient, public state: State, public router: Router) { }

  ngOnInit() {
    this.getAnimalSpeciesFeed()
  }

  async getAnimalSpeciesFeed(){
    try{
    const result = await firstValueFrom(this.http.get<AnimalSpeciesFeed[]>('http://localhost:5000/api/animalspeciesfeed'));
    console.log(result);
    this.state.animalSpeciesFeed = result!;
    }catch(error){
      console.error('Error fetching data:', error)
    }
  }

  goToSpecies(animalNumber: number){
    this.state.currentAnimalSpecies.speciesID = animalNumber;
    this.router.navigate(['/species/' + animalNumber])
  }

  protected readonly AnimalSpeciesFeed = AnimalSpeciesFeed;
}
