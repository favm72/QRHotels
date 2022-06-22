import { Component, Input, OnChanges, OnInit } from "@angular/core"

@Component({
	selector: "app-detail",
	templateUrl: "./detail.component.html",
	styleUrls: ["./detail.component.css"],
})
export class DetailComponent implements OnInit, OnChanges {
	@Input() detail = []

	constructor() {}
	ngOnChanges(changes): void {
		if (changes["detail"]) {
			for (const d of this.detail) {
				if (d.modifier) {
					d.modifiers = JSON.parse(d.modifier).map(m => {
						if (m.type == "radio") {
							return {
								title: m.title,
								value: m.value,
							}
						} else if (m.type == "checkbox") {
							return {
								title: m.title,
								value: m.items
									.filter(x => x.value == true)
									.map(x => x.name)
									.join(", "),
							}
						} else {
							return {
								title: m.title,
								value: m.value,
							}
						}
					})
				}
			}
		}
	}
	ngOnInit(): void {}
}
