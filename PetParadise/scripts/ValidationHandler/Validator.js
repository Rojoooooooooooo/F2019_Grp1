function Validator(element) {
    this.previousErrors = [];
    this.ErrorMessages = [];
    this.parentElement = element;
    this.entry = 0;
}

Validator.prototype.addError = function (error) {
    this.ErrorMessages.push(error);
}

Validator.prototype.removeError = function (property) {
    this.ErrorMessages = this.ErrorMessages.filter(err=>err.property != property);
    
}

Validator.prototype.removeErrors = function () {
    this.previousErrors = this.ErrorMessages;
    this.ErrorMessages = [];
    this.updateParent();
}


Validator.prototype.updateParent = function () {
    const element = document.querySelector(this.parentElement);
    element.innerHTML = "";
    this.ErrorMessages.forEach(error=> {
        const li = document.createElement('li');
        li.class = "text-danger";
        li.innerText = error.message;
        element.appendChild(li);
    });
    
}

Validator.prototype.hasChanged = function () {
    const prev = this.previousErrors;
    const curr = this.ErrorMessages;
    return JSON.stringify(prev) != JSON.stringify(curr);
}