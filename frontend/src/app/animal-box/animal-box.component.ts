import { Component, OnInit } from '@angular/core';
import {HttpClient} from "@angular/common/http";
import {AnimalSpeciesFeed} from "../../models";
import {firstValueFrom} from "rxjs";
import {State} from "../../state";


@Component({
  selector: 'app-animal-box',
  templateUrl: './animal-box.component.html',
  styleUrls: ['./animal-box.component.scss'],

})
export class AnimalBoxComponent  implements OnInit {

  constructor(public http: HttpClient, public state: State) { }

  ngOnInit() {
    this.getAnimalSpeciesFeed()
  }

  async getAnimalSpeciesFeed(){
    try{
    const result = await firstValueFrom(this.http.get<AnimalSpeciesFeed[]>('http://localhost:5163/api/animalspeciesfeed'));
    console.log(result);
    this.state.animalSpeciesFeed = result!;
    }catch(error){
      console.error('Error fetching data:', error)
    }
  }

}
