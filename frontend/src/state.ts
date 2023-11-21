import {Injectable} from "@angular/core";
import {AnimalSpecies, AnimalSpeciesFeed} from "./models";

@Injectable({
  providedIn: 'root'
})

export class State{
  animalSpeciesFeed: AnimalSpeciesFeed[] = [];
  currentAnimalSpecies: AnimalSpecies = new AnimalSpecies();
}
