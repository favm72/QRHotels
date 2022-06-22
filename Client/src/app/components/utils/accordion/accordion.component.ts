import { Component, Input, OnInit } from "@angular/core"

@Component({
	selector: "app-accordion",
	templateUrl: "./accordion.component.html",
	styleUrls: ["./accordion.component.css"],
})
export class AccordionComponent implements OnInit {
	active: boolean = false
	@Input() title: string

	constructor() {}

	ngOnInit(): void {}

	toggle() {
		this.active = !this.active
	}
}
