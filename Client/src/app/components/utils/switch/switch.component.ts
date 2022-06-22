import { Component, EventEmitter, Input, OnInit, Output } from "@angular/core"

@Component({
	selector: "app-switch",
	templateUrl: "./switch.component.html",
	styleUrls: ["./switch.component.css"],
})
export class SwitchComponent implements OnInit {
	@Input() checked: boolean
	//@Output() change: EventEmitter<any> = new EventEmitter()

	constructor() {}

	// handleChange(event): void {
	// 	//this.change.emit(event)
	// }

	ngOnInit(): void {}
}
