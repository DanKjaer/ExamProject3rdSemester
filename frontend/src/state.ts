import {Injectable} from "@angular/core";
import {AnimalFeed, AnimalNote, AnimalNoteFeed, Animals, AnimalSpecies, AnimalSpeciesFeed, Users} from "./models";

@Injectable({
  providedIn: 'root'
})

export class State{
  animalSpeciesFeed: AnimalSpeciesFeed[] = [];
  currentAnimalSpecies: AnimalSpecies = new AnimalSpecies();
  animalFeed: AnimalFeed[] = [];
  currentAnimal: Animals = new Animals();
  user: Users[] = [];
  currentUser: Users = new Users();
  selectedUser: Users = new Users();
  currentAnimalNote: AnimalNote = new AnimalNote();
  animalNoteFeed: AnimalNoteFeed[] = [];

}
