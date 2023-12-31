import {Component, OnInit} from '@angular/core';
import {FormBuilder, Validators} from "@angular/forms";
import {HttpClient, HttpErrorResponse} from "@angular/common/http";
import {AnimalFeed, Animals, AnimalSpecies} from "../../models";
import {firstValueFrom} from "rxjs";
import {State} from "../../state";
import {ModalController, ToastController} from "@ionic/angular";
import {ActivatedRoute} from "@angular/router";

@Component({
  selector: 'app-species',
  templateUrl: './species.component.html',
  styleUrls: ['./species.component.scss'],
})
export class SpeciesComponent implements OnInit {

  createNewAnimalForm = this.fb.group({
    speciesID: ['', Validators.required],
    animalName: ['', Validators.required],
    animalBirthday: ['', Validators.required],
    animalGender: ['', Validators.required],
    animalPicture: [''],
    animalWeight: ['', Validators.required],
    animalDead: [false]
  })

  updateSpeciesForm = this.fb.group({
    speciesName: ['', Validators.required],
    speciesDescription: ['', Validators.required],
    speciesPicture: ['']
  })

  emailForm = this.fb.group({
    animalSpeciesId: ['', Validators.required],
    toAddress: ['', Validators.required],
    toName: ['', Validators.required]
  })

  constructor(public fb: FormBuilder, public http: HttpClient, public state: State, public toastController: ToastController, public route: ActivatedRoute, public modal: ModalController) {
  }

  speciesId?: string | null;
  ngOnInit() {
    this.getSpecies();
  }

  async getSpecies() {
    this.route.paramMap.subscribe(params => {
      this.speciesId = params.get('id');
    })
    const result = await firstValueFrom(this.http.get<AnimalSpecies>('https://moonhzoo.azurewebsites.net/api/animalspecies/' + this.speciesId));
    this.state.currentAnimalSpecies = result;
    this.initializeEdit();
  }

  async createAnimal() {
    try {
      let dto = this.createNewAnimalForm.getRawValue();
      this.route.paramMap.subscribe(params => {
        dto.speciesID = params.get('id');
      });
      const observable = this.http.post<Animals>('https://moonhzoo.azurewebsites.net/api/animal', dto);
      const response = await firstValueFrom(observable);
      this.state.animalFeed.push(<AnimalFeed>response);
      await this.modal.dismiss();
    } catch (e) {
      if (e instanceof HttpErrorResponse) {
        this.toastController.create({message: e.error.messageToCient}).then(res => res.present)
      }
    }
  }

  initializeEdit() {
    this.updateSpeciesForm.patchValue({
      speciesName: this.state.currentAnimalSpecies.speciesName,
      speciesDescription: this.state.currentAnimalSpecies.speciesDescription,
      speciesPicture: this.state.currentAnimalSpecies.speciesPicture
    });
  }

  async updateSpecies() {
    let dto = this.updateSpeciesForm.getRawValue();
    const observable = this.http.put<AnimalSpecies>('https://moonhzoo.azurewebsites.net/api/animalspecies', dto);
    const response = await firstValueFrom(observable);
    this.state.currentAnimalSpecies = response;
    await this.modal.dismiss();
  }

  async sendMail() {
    let dto = this.emailForm.getRawValue();
    this.route.paramMap.subscribe(params => {
      dto.animalSpeciesId = params.get('id');
    });
    const observable = this.http.post('https://moonhzoo.azurewebsites.net/api/sendEmail', dto);
    const response = await firstValueFrom(observable);
    await this.modal.dismiss();
    await this.toastController.create({message: response.toString(), duration: 5000}).then(res => res.present);
  }

}
