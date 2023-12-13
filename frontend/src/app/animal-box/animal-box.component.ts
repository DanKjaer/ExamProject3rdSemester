import { Component, OnInit } from '@angular/core';
import {HttpClient} from "@angular/common/http";
import {AnimalNote, AnimalSpecies, AnimalSpeciesFeed} from "../../models";
import {firstValueFrom} from "rxjs";
import {State} from "../../state";
import {Router} from "@angular/router";
import { TokenService } from 'src/services/token.services';
import {FormBuilder, Validators} from "@angular/forms";


@Component({
  selector: 'app-animal-box',
  templateUrl: './animal-box.component.html',
  styleUrls: ['./animal-box.component.scss'],

})
export class AnimalBoxComponent  implements OnInit {

  constructor(public http: HttpClient, public state: State, public router: Router,
              private readonly token: TokenService, public fb: FormBuilder)
    {
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
        if(!this.token.getToken()){
          this.router.navigateByUrl("/login");
        }
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

  async goToSpecies(animalNumber: number) {
    this.state.currentAnimalSpecies.speciesID = animalNumber;
    const result = await firstValueFrom(this.http.get<AnimalSpecies>('http://localhost:5000/api/animalspecies/' + this.state.currentAnimalSpecies.speciesID))
    this.state.currentAnimalSpecies = result;
    this.router.navigate(['/species/' + animalNumber])
  }

  async createSpecies() {
    let dto = this.createSpeciesForm.getRawValue();
    const observable = await this.http.post<AnimalSpecies>('http://localhost:5000/api/animalspecies', dto);
    const response = await firstValueFrom(observable);
    this.state.animalSpeciesFeed.push(response);
  }
}
