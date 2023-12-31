import {Component, OnInit} from '@angular/core';
import {HttpClient} from "@angular/common/http";
import {AnimalNote, AnimalSpecies, AnimalSpeciesFeed} from "../../models";
import {firstValueFrom} from "rxjs";
import {State} from "../../state";
import {Router} from "@angular/router";
import {TokenService} from 'src/services/token.services';
import {FormBuilder, Validators} from "@angular/forms";
import {ModalController} from "@ionic/angular";


@Component({
  selector: 'app-animal-box',
  templateUrl: './animal-box.component.html',
  styleUrls: ['./animal-box.component.scss'],

})
export class AnimalBoxComponent implements OnInit {

  constructor(public http: HttpClient, public state: State, public router: Router,
              private readonly token: TokenService, public fb: FormBuilder, public modal: ModalController) {
    this.checkIfLogin();
  }

  createSpeciesForm = this.fb.group({
    speciesName: ['', Validators.required],
    speciesDescription: ['', Validators.required],
    speciesPicture: ['', Validators.required]
  });

  ngOnInit() {
    this.getAnimalSpeciesFeed();
  }

  checkIfLogin() {
    if (!this.token.getToken()) {
      this.router.navigateByUrl("/login");
    }
  }

  async getAnimalSpeciesFeed() {
    try {
      const result = await firstValueFrom(this.http.get<AnimalSpeciesFeed[]>('https://moonhzoo.azurewebsites.net/api/animalspeciesfeed'));
      this.state.animalSpeciesFeed = result!;
    } catch (error) {
      console.error('Error fetching data:', error)
    }
  }

  async goToSpecies(animalNumber: number) {
    this.state.currentAnimalSpecies.speciesID = animalNumber;
    const result = await firstValueFrom(this.http.get<AnimalSpecies>('https://moonhzoo.azurewebsites.net/api/animalspecies/' + this.state.currentAnimalSpecies.speciesID))
    this.state.currentAnimalSpecies = result;
    this.router.navigate(['/species/' + animalNumber])
  }

  async createSpecies() {
    let dto = this.createSpeciesForm.getRawValue();
    const observable = this.http.post<AnimalSpecies>('https://moonhzoo.azurewebsites.net/api/animalspecies', dto);
    const response = await firstValueFrom(observable);
    this.state.animalSpeciesFeed.push(response);
    await this.modal.dismiss();
  }
}
