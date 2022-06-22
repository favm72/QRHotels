import { Component, Input, OnInit } from "@angular/core"

@Component({
	selector: "app-modal",
	templateUrl: "./modal.component.html",
	styleUrls: ["./modal.component.css"],
})
export class ModalComponent implements OnInit {
	active: boolean = false
	type: string = "neutral"
	@Input() showFooter: boolean = false
	@Input() width: string = ""

	constructor() {}

	ngOnInit(): void {}

	open() {
		this.active = true
	}

	close() {
		this.active = false
	}
}
