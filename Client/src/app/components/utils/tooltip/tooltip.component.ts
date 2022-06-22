import { Component, Input, OnInit } from "@angular/core"

@Component({
	selector: "app-tooltip",
	templateUrl: "./tooltip.component.html",
	styleUrls: ["./tooltip.component.css"],
})
export class TooltipComponent implements OnInit {
	@Input() show: boolean
	@Input() tip: string
	@Input() type: number = 1

	constructor() {}

	ngOnInit(): void {}
}
