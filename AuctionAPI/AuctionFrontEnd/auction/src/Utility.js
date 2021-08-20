
export class Util{
    static decomposeDates(d1, d2){
        // get total seconds between the times
        var delta = Math.abs(d2 - d1) / 1000;

        // calculate (and subtract) whole days
        var days = Math.floor(delta / 86400);
        delta -= days * 86400;

        // calculate (and subtract) whole hours
        var hours = Math.floor(delta / 3600) % 24;
        delta -= hours * 3600;

        // calculate (and subtract) whole minutes
        var minutes = Math.floor(delta / 60) % 60;
        delta -= minutes * 60;

        // what's left is seconds
        var seconds = Number.parseInt( (delta % 60).toFixed(0) );  // in theory the modulus is not required
        return {days,hours,minutes,seconds};
    }
    static decomposeDate(d1){
        return this.decomposeDates(d1,new Date())
    }
    static formatDate(d){
        let result = d.toLocaleTimeString([], { hour: '2-digit', minute: '2-digit' }) + " " + d.toLocaleDateString()
        return result;
    }
}