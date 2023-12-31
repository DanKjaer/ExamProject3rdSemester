export class AnimalSpeciesFeed{
  speciesID!: number;
  speciesName?: string;
  speciesPicture?: string;
}

export class AnimalSpecies{
  speciesID!: number;
  speciesName?: string;
  speciesDescription?: string;
  speciesPicture?: string;
}

export class Animals{
  animalID?: number;
  speciesID?: number;
  animalName?: string;
  animalBirthday!: Date;
  animalGender?: boolean;
  animalDead?: boolean;
  animalPicture?: string;
  animalWeight?: number;
}

export class AnimalFeed{
  animalID!: number;
  speciesID!: number;
  animalName?: string;
}

export class LoginResponse {
  token!: string;
}

export class Users {
  userID!: number;
  userName!: string;
  userEmail!: string;
  phoneNumber?: string;
  disabled?: boolean;
  toBeDisabledDate?: Date;
  userType?: UserType;
  picture?: string;
}

export class AnimalNote{
  animalID!: number;
  noteText!: string;
}

export class AnimalNoteFeed{
  noteID!: number;
  animalID!: number;
  noteText!: string;
  noteDate!: Date;
}

export enum UserType{
  manager,
  dyrepasser,
  elev
}

export class AnimalSearchFeed{
  animalID!: number;
  animalName!: string;
}

export class AnimalSpeciesSearchFeed{
  speciesID!: number;
  speciesName!: string;
  speciesPicture?: string;
}

export class SearchFeed{
  AnimalSpecies?: AnimalSpeciesSearchFeed[];
  Animals?: AnimalSearchFeed[];
}
