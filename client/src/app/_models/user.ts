export interface User {
    username: string;
    token: string;
    photoUrl: string;
    knownAs: string;
    gender: string;
    roles: string[];
}

/*
let data: number | string = "32";
data = 65;

interface Car {
    color: string;
    model: string;
    topSpeed: number
}

const car1: Car = {
    color: "red",
    model: 'BMW',
    topSpeed: 65
}

const car2: Car = {
    color: 'blue',
    model: 'Mercedes',
    topSpeed: 76
}


const multiply = (x: number, y: number) => {
    return x * y;
}
*/