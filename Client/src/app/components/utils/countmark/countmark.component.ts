import { Component, Input, OnInit } from "@angular/core"

@Component({
	selector: "app-countmark",
	templateUrl: "./countmark.component.html",
	styleUrls: ["./countmark.component.css"],
})
export class CountmarkComponent implements OnInit {
	@Input() count: number = 0

	constructor() {}

	ngOnInit(): void {}
}
