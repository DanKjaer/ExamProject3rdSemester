import { Component, OnInit } from '@angular/core';
import {firstValueFrom} from "rxjs";
import {HttpClient} from "@angular/common/http";
import {State} from "../../state";
import {ActivatedRoute} from "@angular/router";
import {Animals, AnimalSpecies} from "../../models";

@Component({
  selector: 'app-animals',
  templateUrl: './animals.component.html',
  styleUrls: ['./animals.component.scss'],
})
export class AnimalsComponent  implements OnInit {

  animalId?: string | null;
  animalBirthday?: Date;
  animalAge?: number;
  constructor(public http: HttpClient, public state: State, public route: ActivatedRoute) { }

  ngOnInit() {
    this.route.paramMap.subscribe(params => {
      this.animalId = params.get('id')
    })
    this.getAnimal();
  }

  async getAnimal() {
    const result = await firstValueFrom(this.http.get<Animals>('http://localhost:5000/api/animal/' + this.animalId));
    console.log(result)
    this.state.currentAnimal = result;
    this.getSpeciesName(this.state.currentAnimal.speciesID);
    this.animalBirthday = new Date(this.state.currentAnimal.animalBirthday!);
    this.calculateAge();
  }

  async getSpeciesName(speciesId: number | undefined) {
    const result = await firstValueFrom(this.http.get<AnimalSpecies>('http://localhost:5000/api/animalspecies/' + speciesId));
    this.state.currentAnimalSpecies = result;
  }

  calculateAge(): void | null {
    if (!this.animalBirthday || !(this.animalBirthday instanceof Date)) {
      return null;
    }

    let timeDiff = Math.abs(Date.now() - this.animalBirthday?.getTime());
    let age = Math.floor(timeDiff / (1000*3600*24*365));
    this.animalAge = age;
  }

}
