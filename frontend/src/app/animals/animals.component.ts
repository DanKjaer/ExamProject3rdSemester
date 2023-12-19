import {booleanAttribute, Component, OnInit} from '@angular/core';
import {firstValueFrom} from "rxjs";
import {HttpClient} from "@angular/common/http";
import {State} from "../../state";
import {ActivatedRoute} from "@angular/router";
import {AnimalNote, AnimalNoteFeed, Animals, AnimalSpecies} from "../../models";
import {FormBuilder, Validators} from "@angular/forms";
import {toggle} from "ionicons/icons";
import {ModalController} from "@ionic/angular";

@Component({
  selector: 'app-animals',
  templateUrl: './animals.component.html',
  styleUrls: ['./animals.component.scss'],
})
export class AnimalsComponent implements OnInit {

  createAnimalNoteForm = this.fb.group({
    animalID: [0, [Validators.required, Validators.pattern("^[0-9]*$"), Validators.min(0)]],
    noteText: ['', Validators.required, Validators.minLength(3)]
  })

  updateAnimalForm = this.fb.group({
    animalID: [0, [Validators.required, Validators.pattern("^[0-9]*$"), Validators.min(0)]],
    speciesID: [0, [Validators.required, Validators.pattern("^[0-9]*$"), Validators.min(0)]],
    animalName: ['', Validators.required],
    animalBirthday: [new Date(), Validators.required],
    animalGender: [false, Validators.required],
    animalDead: [false, Validators.required],
    animalPicture: [''],
    animalWeight: [0, [Validators.required, Validators.pattern("^[0-9]*$"), Validators.min(0)]]
  })

  animalId?: string | null;
  animalBirthday?: Date;
  animalAge?: number;

  constructor(public http: HttpClient, public state: State, public route: ActivatedRoute, public fb: FormBuilder, public modal: ModalController) {
  }

  ngOnInit() {
    this.route.paramMap.subscribe(params => {
      this.animalId = params.get('id')
    })
    this.getAnimal();
  }

  async getAnimal() {
    const result = await firstValueFrom(this.http.get<Animals>('https://moonhzoo.azurewebsites.net/api/animal/' + this.animalId));
    this.state.currentAnimal = result;
    this.getSpeciesName(this.state.currentAnimal.speciesID);
    this.animalBirthday = new Date(this.state.currentAnimal.animalBirthday!);
    this.calculateAge();
    this.getNote();
    this.initializeEdit();
  }

  initializeEdit() {
    this.updateAnimalForm.patchValue({
      animalName: this.state.currentAnimal.animalName,
      animalWeight: this.state.currentAnimal.animalWeight,
      animalPicture: this.state.currentAnimal.animalPicture,
      animalGender: this.state.currentAnimal.animalGender,
      animalDead: this.state.currentAnimal.animalDead,
      //virker men viser det ikke ¯\_(ツ)_/¯
      animalBirthday: this.state.currentAnimal.animalBirthday
    });
  }

  async getSpeciesName(speciesId: number | undefined) {
    const result = await firstValueFrom(this.http.get<AnimalSpecies>('https://moonhzoo.azurewebsites.net/api/animalspecies/' + speciesId));
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
    const result = await firstValueFrom(this.http.get<AnimalNoteFeed[]>('https://moonhzoo.azurewebsites.net/api/animalnote/' + this.state.currentAnimal.animalID));
    this.state.animalNoteFeed = result!;
  }

  clearNoteInput() {
    this.createAnimalNoteForm.get('noteText')?.setValue('');
  }

  async saveNote() {
    let dto = this.createAnimalNoteForm.getRawValue();
    dto.animalID = Number(this.state.currentAnimal.animalID);
    const observable = await this.http.post<AnimalNote>('https://moonhzoo.azurewebsites.net/api/animalnote/', dto);
    const result = await firstValueFrom(observable);
    this.state.animalNoteFeed.push(<AnimalNoteFeed>result);
    this.clearNoteInput();
  }

  async deleteNote(noteId: number) {
    await firstValueFrom(this.http.delete('https://moonhzoo.azurewebsites.net/api/animalnote/' + noteId));
    this.state.animalNoteFeed = this.state.animalNoteFeed.filter(note => note.noteID !== noteId);
  }


  async updateAnimal() {
    let dto = this.updateAnimalForm.getRawValue();
    dto.animalID = Number(this.state.currentAnimal.animalID);
    dto.speciesID = Number(this.state.currentAnimal.speciesID);
    const observable = this.http.put<Animals>('https://moonhzoo.azurewebsites.net/api/animal', dto);
    const result = await firstValueFrom(observable);
    this.state.currentAnimal = result;
    await this.modal.dismiss();
  }
}
