import {Component, OnInit} from '@angular/core';
import {firstValueFrom} from "rxjs";
import {HttpClient} from "@angular/common/http";
import {State} from "../../state";
import {ActivatedRoute} from "@angular/router";
import {AnimalNote, AnimalNoteFeed, Animals, AnimalSpecies} from "../../models";
import {FormBuilder, Validators} from "@angular/forms";
import {toggle} from "ionicons/icons";

@Component({
  selector: 'app-animals',
  templateUrl: './animals.component.html',
  styleUrls: ['./animals.component.scss'],
})
export class AnimalsComponent implements OnInit {

  createAnimalNoteForm = this.fb.group({
    animalsID: [0, [Validators.required, Validators.pattern("^[0-9]*$"), Validators.min(0)]],
    noteText: ['', Validators.required]
  })

  animalId?: string | null;
  animalBirthday?: Date;
  animalAge?: number;

  constructor(public http: HttpClient, public state: State, public route: ActivatedRoute, public fb: FormBuilder) {
  }

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
    this.getNote();
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
    let age = Math.floor(timeDiff / (1000 * 3600 * 24 * 365));
    this.animalAge = age;
  }

  async getNote() {
    const result = await firstValueFrom(this.http.get<AnimalNoteFeed[]>('http://localhost:5000/api/animalnote/' + this.state.currentAnimal.animalID));
    this.state.animalNoteFeed = result!;
  }

  async saveNote() {
    let dto = this.createAnimalNoteForm.getRawValue();
    dto.animalsID = Number(this.state.currentAnimal.animalID);
    
    let animalnote = new AnimalNote;
    animalnote.animalID = this.state.currentAnimal.animalID!
    animalnote.noteText = this.createAnimalNoteForm.getRawValue().noteText!
    console.log("weird way: ", animalnote)
    console.log("sensible way: " ,dto)
    const observable = await this.http.post<AnimalNote>('http://localhost:5000/api/animalnote/', animalnote);
    const result = await firstValueFrom(observable);
    this.state.animalNoteFeed.push(<AnimalNoteFeed>result);

  }

}
