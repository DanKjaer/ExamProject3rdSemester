import { Component, OnInit } from '@angular/core';
import {firstValueFrom} from "rxjs";
import {HttpClient} from "@angular/common/http";
import {State} from "../../state";
import {ActivatedRoute} from "@angular/router";
import {AnimalNote, Animals, AnimalSpecies} from "../../models";
import {toggle} from "ionicons/icons";

@Component({
  selector: 'app-animals',
  templateUrl: './animals.component.html',
  styleUrls: ['./animals.component.scss'],
})
export class AnimalsComponent  implements OnInit {

  animalId?: string | null;
  animalBirthday?: Date;
  animalAge?: number;
  toggleEditButtons: boolean = false;
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
    this.getNote(this.state.currentAnimal.animalID);
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

  async getNote(animalId: number | undefined) {
    const result = await firstValueFrom(this.http.get<AnimalNote>('http://localhost:5000/api/animalnote/' + animalId));
    this.state.currentAnimalNote = result;
  }

  toggleEdit() {
    this.toggleEditButtons = !this.toggleEditButtons;
    return this.toggleEditButtons;
  }

  saveNote() {
    const textField: HTMLElement | null = document.getElementById('text-area');
    if (this.state.currentAnimalNote.noteID == null) {
      const dto = new AnimalNote();
      this.http.post('https://localhost:5000/api/animalnote/', textField?.textContent)
    }

    this.http.put('https://localhost:5000/api/animalnote/', textField?.textContent);
  }

  protected readonly toggle = toggle;
}
